@extends('layout')

@section('content')
    <h1 class="title">Register a new Vehicle</h1>

    <form method="POST" action="/vehicles">
        {{ csrf_field() }}
        <div>
            <input type="text" name="license_plate" 
                class="input {{ $errors->has('license_plate') ? 'is-danger' : '' }}" 
                placeholder="Enter your licence plate" required
                value="{{ old('license_plate') }}"/>
            <input type="text" name="name" 
                class="input {{ $errors->has('name') ? 'is-danger' : '' }}" 
                placeholder="Enter a name for your vehicle" required
                value="{{ old('name') }}"/>
        </div>
        <div>
            <button type="submit" class="button is-link">Add Vehicle</button>
        </div>

        @include('errors')
    </form>
@endsection