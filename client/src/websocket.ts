import { host } from './env'

// Exported function for sending messages
export let send: (data: any) => any

// Function that is called when we receive a message
let onMessageCallback: (data: any) => any

// Timeout for connection retry
let timeout = 250

// Exported function for initializing the websocket connection
export const startWebsocketConnection = () => {
	// A new Websocket connection is initialized with the server
	const ws = new WebSocket('ws://' + host + '/chat')

	// If the connection is successfully opened
	ws.onopen = () => {
        console.log('Socket connection is opened.')
        // Reset the timer back to 250s
        timeout = 250
	}

	// If the connection is closed
	ws.onclose = (e) => {
        console.log('Socket is closed. Reconnect will be attempted. Details: ', e.code, e.reason)
        setTimeout(() => {
            startWebsocketConnection()
        }, Math.min(10000, timeout += timeout))
    }
    
    // If there was an error
    ws.onerror = () => {
        console.error("Socket encountered error. Closing the socket.")
        ws.close()
    }

	// Callback is called everytime a message is received
	ws.onmessage = (e) => {
		onMessageCallback(e.data)
	}

	// We assign the send method of the connection to the exported send function
	send = ws.send.bind(ws)
}

// Function that registers a callback that needs to be called everytime a new message is received
export const registerOnMessageCallback = (f: (data: any) => any) => {
	onMessageCallback = f
}
