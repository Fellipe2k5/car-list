import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { FiPower, FiEdit, FiTrash2 } from 'react-icons/fi'

import api from '../../services/api';

import './styles.css';

export default function Cars(){

    const [cars, setCars] = useState([]);
    const [page, setPage] = useState(1);

    const userName = localStorage.getItem('userName');
    const accessToken = localStorage.getItem('accessToken');

    const authorization = {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    }

    const history = useHistory();

    useEffect(() => {
        fetchMoreCars();
    }, [accessToken]);

    async function fetchMoreCars() {
        const response = await api.get(`api/Car/v1/asc/4/${page}`, authorization);
        setCars([ ...cars, ...response.data.list]);
        setPage(page + 1);
    }
    
    async function logout() {
        try {
            await api.get('api/auth/v1/revoke', authorization);

            localStorage.clear();
            history.push('/');
        } catch (err) {
            alert('Logout failed! Try again!');
        }
    }
    
    async function editCar(id) {
        try {
            history.push(`car/new/${id}`)
        } catch (err) {
            alert('Edit car failed! Try again!');
        }
    }

    async function deleteCar(id) {
        try {
            await api.delete(`api/Car/v1/${id}`, authorization);

            setCars(cars.filter(car => car.id !== id))
        } catch (err) {
            alert('Delete failed! Try again!');
        }
    }

    return (
        <div className="car-container">
            <header>              
                <span>Welcome, <strong>{userName.toLowerCase()}</strong>!</span>
                <Link className="button" to="car/new/0">Add New Car</Link>
                <button onClick={logout} type="button">
                    <FiPower size={18} color="#251FC5" />
                </button>
            </header>

            <h1>Registered Cars</h1>
            <ul>
                {cars.map(car => (
                    <li key={car.id}>
                        <strong>Make:</strong>
                        <p>{car.make}</p>
                        <strong>Model:</strong>
                        <p>{car.model}</p>
                        <strong>Color:</strong>
                        <p>{car.color}</p>
                        <strong>Plate:</strong>
                        <p>{car.plate}</p>                    
                        <strong>Date Make:</strong>
                        <p>{Intl.DateTimeFormat('en-US').format(new Date(car.dateMake))}</p>
                        
                        <button onClick={() => editCar(car.id)} type="button">
                            <FiEdit size={20} color="#251FC5"/>
                        </button>
                        
                        <button onClick={() => deleteCar(car.id)} type="button">
                            <FiTrash2 size={20} color="#251FC5"/>
                        </button>
                    </li>
                ))}
            </ul>
            <button className="button" onClick={fetchMoreCars} type="button">Load More</button>
        </div>
    );
}