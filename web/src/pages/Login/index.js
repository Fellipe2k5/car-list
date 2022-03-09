import React, {useState} from 'react';
import { useHistory } from 'react-router-dom';

import api from '../../services/api';

import './styles.css';

import carLogo from '../../assets/carLogo.png'

export default function Login() {

    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const history = useHistory();

    async function login(e) {
        e.preventDefault();

        const data = {
            userName,
            password,
        };

        try {
            const response = await api.post('api/auth/v1/signin', data);

            localStorage.setItem('userName', userName);
            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);

            history.push('/cars');
        } catch (error) {
            alert('Login failed! Try again!');
        }

    }

    return (
        <div className="login-container">
            <section className="form">     
                <form onSubmit={login}>
                    <h1>Access your Account</h1>

                    <input
                        placeholder="Username"
                        value={userName}
                        onChange={e => setUserName(e.target.value)}
                    />

                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                    />

                    <button className="button" type="submit">Login</button>
                </form>

            </section>

            <img src={carLogo} alt="Login"/>

        </div>
    )

}