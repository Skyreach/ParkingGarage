<?php

namespace App\Services;

use App\Ticket;
use App\Location;

class TicketService
{
    public function amountOwed(Ticket $ticket)
    {
        $location = Location::findOrFail(1);

        $rate = $location->rate;
        $rateIncreasePercent = $location->rate_increase_percentage;
        $rateDurations = $location->rate_increase_durations;
        $issued_at = new DateTime($ticket->created_at);
        $sinceIssued = $issued_at->diff(new DateTime(date("Y-m-d H:i:s")));
        
        foreach(explode(',', $rateDurations) as $intervalDuration)
        {
            if ($sinceIssued->i > $intervalDuration)
            {
                $rate = $rate * (1 + $rateIncreasePercent);
            } 
            else
            {
                break;
            }
        }
        
        return $rate;
    }

}