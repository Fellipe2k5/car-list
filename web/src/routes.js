import React from 'react';
import {BrowserRouter, Route, Switch} from 'react-router-dom';

import Login from './pages/Login';
import Cars from './pages/Cars';
import NewCar from './pages/NewCar';


export default function Routes() {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={Login}/>
                <Route path="/cars" component={Cars}/>
                <Route path="/car/new/:carId" component={NewCar}/>
            </Switch>
        </BrowserRouter>
    );
}