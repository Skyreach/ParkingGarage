<?php

namespace App\Policies;

use App\User;
use App\Ticket;
use Illuminate\Auth\Access\HandlesAuthorization;

class TicketPolicy
{
    use HandlesAuthorization;

    public function update(?User $user, Ticket $ticket)
    {
        return $ticket->owner_id == $user->id;
    }
}
