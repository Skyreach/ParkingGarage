@extends('layout')

@section('content')
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">Amount owed - {{ $amount }}</div>
                <div class="card-body">
                    Issued: {{ $ticket->created_at }}
                    Amount Owing: {{ $amount }}
                    
                    <form method="POST" action="/payments/{{ $ticket->id }}">
                        @csrf
                        <!-- could also be a patch, because it's updating the ticket -->
                        <input type="text" name="credit_info" 
                            class="input {{ $errors->has('credit_info') ? 'is-danger' : '' }}" 
                            placeholder="Enter your card number" required
                            value="{{ old('credit_info') }}"/>
                            <button type="submit" class="button is-link">Pay ticket</button>
                    </form>
                    @include ('errors')
                </div>
            <div>
        </div>
    </div>
</div>
@endsection
