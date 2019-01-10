<!DOCTYPE html>
<html>
<head>
    <title>@yield('title', 'Laracasts')</title>

    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,600" rel="stylesheet" type="text/css">

    <!-- Styles -->
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/bulma/0.7.2/css/bulma.css">
    <link rel="stylesheet" type="text/css" href="{{ mix('css/app.css') }}">
    <style>
            html, body {
                background-color: #fff;
                color: #636b6f;
                font-family: 'Nunito', sans-serif;
                font-weight: 200;
                height: 100vh;
                margin: 0;
            }

            .full-height {
                height: 50vh;
            }

            .flex-center {
                align-items: center;
                display: flex;
                justify-content: center;
            }

            .position-ref {
                position: relative;
            }

            .top-right {
                position: absolute;
                right: 10px;
                top: 18px;
            }

            .content {
                text-align: center;
            }

            .title {
                font-size: 84px;
            }

            .links > a {
                color: #636b6f;
                padding: 0 25px;
                font-size: 13px;
                font-weight: 600;
                letter-spacing: .1rem;
                text-decoration: none;
                text-transform: uppercase;
            }

            .m-b-md {
                margin-bottom: 30px;
            }
        </style>
</head>

<body>
<div id="app" class="flex-center position-ref full-height">
    <!-- <example-component></example-component> -->
    @if (Route::has('login'))
        <div class="top-right links">
            @auth
                <a href="{{ url('/welcome') }}">Home</a>
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