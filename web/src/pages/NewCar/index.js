import React, { useEffect, useState } from 'react';
import { Link, useHistory, useParams } from 'react-router-dom';
import { FiArrowLeft } from 'react-icons/fi'

import api from '../../services/api';

import './styles.css';

export default function NewCar(){

    const [id, setId] = useState(null);
    const [make, setMake] = useState('');
    const [model, setModel] = useState('');    
    const [color, setColor] = useState('');
    const [plate, setPlate] = useState('');
    const [dateMake, setDateMake] = useState('');

    const { carId } = useParams();

    const history = useHistory();

    const accessToken = localStorage.getItem('accessToken');

    const authorization = {
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    }

    useEffect(() => {
        if(carId === '0') return;
        else loadCar();
    }, carId);

    async function loadCar() {
        try {
            const response = await api.get(`api/car/v1/${carId}`, authorization)

            let adjustedDate = response.data.dateMake.split("T", 10)[0];

            setId(response.data.id);
            setMake(response.data.make);
            setModel(response.data.model);
            setColor(response.data.color);
            setPlate(response.data.plate);
            setDateMake(adjustedDate);
        } catch (error) {            
            alert('Error recovering Car! Try again!')
            history.push('/cars');
        }
    }

    async function saveOrUpdate(e) {
        e.preventDefault();

        const data = {
            make,
            model,
            color,
            plate,
            dateMake,
        }

        try {
            if(carId === '0') {
                await api.post('api/Car/v1', data, authorization);
            } else {
                data.id = id;
                await api.put('api/Car/v1', data, authorization);
            }
        } catch (err) {
            alert('Error while recording Car! Try again!')
        }
        history.push('/cars');
    }

    return (
        <div className="new-car-container">
            <div className="content">
                <section className="form">               
                    <h1>{carId === '0'? 'Add New' : 'Update'} Car</h1>
                    <p>Enter the car information and click on {carId === '0'? `'Add'` : `'Update'`}!</p>
                    <Link className="back-link" to="/cars">
                        <FiArrowLeft size={16} color="#251fc5"/>
                        Back to Cars
                    </Link>
                </section>

                <form onSubmit={saveOrUpdate}>
                    <input 
                        placeholder="Make"
                        value={make}
                        onChange={e => setMake(e.target.value)}
                    />
                    <input 
                        placeholder="Model"                        
                        value={model}
                        onChange={e => setModel(e.target.value)}
                    />
                    <input 
                        placeholder="Color"                        
                        value={color}
                        onChange={e => setColor(e.target.value)}
                    />
                    <input 
                        placeholder="Plate"                        
                        value={plate}
                        onChange={e => setPlate(e.target.value)}
                        maxLength="6"
                    />
                    <input 
                        type="date"                        
                        value={dateMake}
                        onChange={e => setDateMake(e.target.value)}
                    />                   

                    <button className="button" type="submit">{carId === '0'? 'Add' : 'Update'}</button>
                </form>
            </div>
        </div>
    );
}