<?php

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::get('/', function () {
    return view('welcome');
});
Route::get('/welcome', function (){
    return view('welcome');
});

Route::get('/vehicles', 'VehicleController@index');
Route::get('/vehicles/create', 'VehicleController@create');
Route::post('/vehicles', 'VehicleController@store');

Route::get('/tickets', 'TicketController@index');
Route::get('/ticket/{ticket}', 'TicketController@show');
Route::post('/ticket', 'TicketController@store');

Route::post('/payments/{ticket}', 'TicketPaymentsController@update');

Auth::routes();

