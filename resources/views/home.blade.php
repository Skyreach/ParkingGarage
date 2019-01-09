@extends('layouts.app')

@section('content')
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">Dashboard</div>

                <div class="card-body">
                    @if (session('status'))
                        <div class="alert alert-success" role="alert">
                            {{ session('status') }}
                        </div>
                    @endif

                    Please add a vehicle to continue

                    <!-- todo: convert to vue -->
                    <a class="button is-link" href="/vehicles/create">Add a vehicle!</a>
                </div>
            </div>
        </div>
    </div>
</div>
@endsection
