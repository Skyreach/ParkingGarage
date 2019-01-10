@extends('layout')

@section('content')
        <div class="content">
            <div class="links">
                @if (Route::has('login'))
                    @auth
                        <a href="{{ url('/tickets') }}">My tickets</a>
                    @else
                        To continue please
                        <a href="{{ route('login') }}">Login</a>
                        or 
                        @if (Route::has('register'))
                            <a href="{{ route('register') }}">Register</a>
                        @endif
                    @endauth
                @endif
            </div>
        </div>

@endsection