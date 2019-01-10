<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Ticket;
use App\Services\TicketService;

class TicketPaymentsController extends Controller
{
    public function update(Request $request, Ticket $ticket)
    {
        $cardNum = $request->getContent();
        $location = Location::findOrFail(1);
        $rate = TicketService.amountOwed($ticket);

        //Some sort of PaymentsService...

    }
}
