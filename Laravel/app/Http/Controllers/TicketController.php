<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use App\Location;
use App\Ticket;
use App\Services\TicketService;

class TicketController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {
        $vehicle = auth()->user()->vehicle()->first();
        // $tickets = auth()->user()->ticket->where('isActive', true)->get();
        $tickets = auth()->user()->ticket()->where('is_active', 1)->get();
        
        return view('tickets.index', [
            'vehicle' => $vehicle,
            'tickets' => $tickets,
        ]);
    }

    public function store() 
    {
        $location = Location::findOrFail(1);
        if ($location->occupancy > 0) {
            // todo: consider introducing polymorphic relationship
            $ticket = new Ticket;
            $ticket->user()->associate(Auth::user());
            $ticket->location()->associate($location);

            $ticket->save();

            return view('tickets.index', [
                'tickets' => $ticket,
            ]);
        }

        return view('tickets.index', [
            'garage_status' => 'full',
        ]);
    }

    public function show(Ticket $ticket)
    {
        $this->authorize('update', $ticket);
        
        $rate = TicketService.amountOwed($ticket);

        return view('tickets.show', [
            'ticket' => $ticket,
            'amount' => $rate,
        ]);
    }
}
