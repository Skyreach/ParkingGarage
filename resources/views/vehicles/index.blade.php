@extends('layout')

@section('content')
    <h1 class="title">My Vehicles</h1>

    <ul>
        @foreach($vehicles as $vehicle)
            <li>
                {{ $vehicle->name }}, {{ $vehicle->license_plate}}
            </li>
        @endforeach
    </ul>
    
    <a class="button is-link" href="/projects/create">Add {{ $vehicles.any() ? 'another' : 'a' }} Vehicle</a>
@endsection