@extends('layout')

@section('content')
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">Parking Garage Dashboard</div>
                <div class="card-body">
                    @if($tickets->count() == 0)
                        <p>Enter the garage to receive a ticket</p>
                    @else
                        @foreach($tickets as $ticket)
                            <div class="card-body">
                                
                            </div>
                        @endforeach
                    @endif
                </div>
            </div>
                <div style="margin-top:5vh;">
                    @if($vehicles->count() == 0)
                        To continue please 

                        <!-- todo: convert to vue -->
                        <a class="button is-link" href="/vehicles/create">Add a vehicle!</a>
                    @else ($vehicles->count() == 1)
                        <form method="POST" action="/ticket">
                            @csrf
                            <input type="submit" class="button is-link" value="Enter Garage">
                        </form>
                    <!-- todo: if multiple vehicles... -->
                    @endif
                <div>
        </div>
    </div>
</div>
@endsection
