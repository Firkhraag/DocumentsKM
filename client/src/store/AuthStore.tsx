import React, { createContext, useContext, useState, useEffect } from 'react'
import httpClient, { tokenKeyName } from '../axios'

const AuthContext = createContext<boolean>(false)

type DispatchContextType = {
    login: (login: string, password: string) => void
    logout: () => void
}

const AuthDispatchContext = createContext(null as DispatchContextType)
export const useIsAuthenticated = () => useContext(AuthContext)
export const useAuthMethods = () => useContext(AuthDispatchContext)

type AuthProviderProps = {
	children: React.ReactNode
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false)

    const login = async (login: string, password: string) => {
        if (
			login.length > 0 &&
			login.length < 100 &&
			password.length > 0 &&
			password.length < 100
		) {
			const response = await httpClient.post('/api/auth/login', {
				login: login,
				password: password,
            })
            localStorage.setItem(tokenKeyName, response.data.token)
            setIsAuthenticated(true)
		} else {
            throw new Error('Неверный логин или пароль')
        }
    }
    const logout = () => {
        localStorage.removeItem(tokenKeyName)
    }
    
    useEffect(() => {
        const fetchData = async () => {
            // const result = await axios.post(protocol + '://' + host + '/api/login')
            // setAuth(result.data)
        }
       
        fetchData()
    }, []);

	return (
		<AuthContext.Provider value={isAuthenticated}>
			<AuthDispatchContext.Provider value={{login, logout}}>
				{children}
			</AuthDispatchContext.Provider>
		</AuthContext.Provider>
	)
}

// import React, { createContext, useState, useEffect } from 'react'
// import User from '../model/User'

// const UserContext = createContext(null);
// export { UserContext };

// export const UserProvider = props => {


// //     User _authenticatedUser;
// //   User get authenticatedUser {
// //     if (_authenticatedUser == null) {
// //       return null;
// //     }
// //     return User.fromUser(_authenticatedUser);
// //   }

// //   bool _isLoading = false;
// //   bool get isLoading => _isLoading;

// //   Timer _authTimer;

// //   PublishSubject<bool> _userSubject = PublishSubject();
// //   PublishSubject<bool> get userSubject => _userSubject;

// //   Future<Map<String, dynamic>> authenticate(String email, String password, [AuthMode mode = AuthMode.Login]) async {
// //     _isLoading = true;
// //     notifyListeners();
// //     final Map<String, dynamic> authData = {
// //       'email': email,
// //       'password': password,
// //       'returnSecureToken': true,
// //     };
// //     http.Response response;
// //     if (mode == AuthMode.Login) {
// //       response = await http.post(
// //         'https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=AIzaSyCeJhNhP3IGpvozoZ6AsIaijerk8jfnJdo',
// //         body: json.encode(authData),
// //       );
// //     } else {
// //       response = await http.post(
// //         'https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key=AIzaSyCeJhNhP3IGpvozoZ6AsIaijerk8jfnJdo',
// //         body: json.encode(authData),
// //       );
// //     }
// //     final Map<String, dynamic> responseData = json.decode(response.body);
// //     bool hasError = true;
// //     String message = 'Something went wrong';
// //     if (responseData.containsKey('idToken')) {
// //       hasError = false;
// //       message = 'Authentication succeded';
// //       _authenticatedUser = User(
// //         id: responseData['localId'],
// //         email: email,
// //         token: responseData['idToken'],
// //       );
// //       setAuthTimeout(int.parse(responseData['expiresIn']));
// //       _userSubject.add(true);
// //       final DateTime now = DateTime.now();
// //       final DateTime expiryTime = now.add(Duration(seconds: int.parse(responseData['expiresIn'])));
// //       final SharedPreferences prefs = await SharedPreferences.getInstance();
// //       prefs.setString('token', responseData['idToken']);
// //       prefs.setString('userEmail', email);
// //       prefs.setString('userId', responseData['localId']);
// //       prefs.setString('expiryTime', expiryTime.toIso8601String());
// //     } else if (responseData['error']['message'] == 'EMAIL_NOT_FOUND'){
// //       message = 'This email was not found.';
// //     } else if (responseData['error']['message'] == 'INVALID_PASSWORD'){
// //       message = 'Invalid password.';
// //     } else if (responseData['error']['message'] == 'EMAIL_EXISTS'){
// //       message = 'This email already exists';
// //     }
// //     _isLoading = false;
// //     notifyListeners();
// //     return {'success': !hasError, 'message': message};
// //   }

// //   void autoAuthenticate() async {
// //     final SharedPreferences prefs = await SharedPreferences.getInstance();
// //     final String token = prefs.getString('token');
// //     // print(token);
// //     if (token != null) {
// //       final DateTime now = DateTime.now();
// //       final String expiryTimeString = prefs.getString('expiryTime');
// //       final DateTime parsedExpiryTime = DateTime.parse(expiryTimeString);
// //       if (parsedExpiryTime.isBefore(now)) {
// //         _authenticatedUser = null;
// //         notifyListeners();
// //         return;
// //       }
// //       final String userEmail = prefs.getString('userEmail');
// //       final String userId = prefs.getString('userId');
// //       final int tokenLifespan = parsedExpiryTime.difference(now).inSeconds;
// //       _authenticatedUser = User(
// //         id: userId,
// //         email: userEmail,
// //         token: token,
// //       );
// //       _userSubject.add(true);
// //       setAuthTimeout(tokenLifespan);
// //       notifyListeners();
// //     }
// //   }

// //   void logout() async {
// //     _authenticatedUser = null;
// //     _authTimer.cancel();
// //     SharedPreferences prefs = await SharedPreferences.getInstance();
// //     prefs.remove('token');
// //     prefs.remove('userEmail');
// //     prefs.remove('userId');
// //     _userSubject.add(false);
// //   }

// //   void setAuthTimeout(int time) {
// //     _authTimer = Timer(
// //       Duration(seconds: time),
// //       logout,
// //     );
// //   }



//     const [user, setUser] = useState(null);

//     return (
//         <UserContext.Provider value={[user, setUser]}>
//             { props.children }
//         </UserContext.Provider>
//     );
// }