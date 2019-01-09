<!DOCTYPE html>
<html>
<head>
    <title>@yield('title', 'Laracasts')</title>
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.2/css/bulma.css">
    <link rel="stylesheet" type="text/css" href="{{ mix('css/app.css') }}">
</head>

<body>
<div id="app" class="flex-center position-ref full-height">
    <example-component></example-component>
    @if (Route::has('login'))
        <div class="top-right links">
            @auth
                <a href="{{ url('/home') }}">Home</a>
            @else
                <a href="{{ route('login') }}">Login</a>

                @if (Route::has('register'))
                    <a href="{{ route('register') }}">Register</a>
                @endif
            @endauth
        </div>
    @endif
    

    <div class="content">
        <div class="title m-b-md">
            Parking Garage
        </div>
        <!-- <div class="links">
            todo: change Enter/Exit garage based on if a ticket is held
            <a class="button is-link" href="/tickets">Enter Garage!</a>
            <a href="/vehicles">Manage Vehicles</a>
            <a href="/tickets">View Tickets</a>
        </div> -->
        <hr/>
        @yield('content')
    </div>

    <script src="{{ mix('/js/manifest.js') }}"></script>
    <script src="{{ mix('/js/vendor.js') }}"></script>
    <script src="{{ mix('/js/app.js') }}"></script>
</body>
</html>