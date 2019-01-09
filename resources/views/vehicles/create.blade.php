@extends('layout')

@section('content')
    <h1 class="title">Register a new Vehicle</h1>

    <form method="POST" action="/vehicles">
        {{ csrf_field() }}
        <div>
            <input type="text" name="license" 
                class="input {{ $errors->has('license') ? 'is-danger' : '' }}" 
                placeholder="Enter your License Plate" required
                value="{{ old('license') }}"/>
        </div>
        <div>
            <button type="submit" class="button is-link">Add Vehicle</button>
        </div>

        @include('errors')
    </form>
@endsection