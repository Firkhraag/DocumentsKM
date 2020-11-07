interface IEnvAggressiveness {
	id: number
	name: string
}

class EnvAggressiveness {
	id: number
	name: string

	constructor(obj?: IEnvAggressiveness) {
		this.id = (obj && obj.id) || 0
		this.name = (obj && obj.name) || ''
	}
}

export default EnvAggressiveness
