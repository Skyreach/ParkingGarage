<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
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
        $vehicles = auth()->user()->vehicle;
        // $tickets = auth()->user()->ticket->where('isActive', true)->get();
        $tickets = auth()->user()->ticket;
        
        return view('tickets.index', [
            'vehicles' => $vehicles,
            'tickets' => $tickets,
        ]);
    }

    public function store() 
    {
        $location = Location::findOrFail(1);
        if ($location->occupancy > 0) {
            // $ticket = auth()->user()->ticket()->create();
            $ticket = Ticket::create();

            $location->ticket()->save($ticket);
            auth()->user()->ticket()->save($ticket);
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
