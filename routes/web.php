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

Route::get('/vehicles', 'VehicleController@index');
Route::get('/vehicles/create', 'VehicleController@create');
Route::post('/vehicles', 'VehicleController@store');

Route::get('/tickets', 'TicketController@index');
Route::get('/tickets/{ticket}', 'TicketController@show');

Route::post('/tickets', 'TicketController@store');

Route::post('/payments/{ticket}', 'TicketPaymentsController@update');

Auth::routes();

Route::get('/home', 'HomeController@index')->name('home');
