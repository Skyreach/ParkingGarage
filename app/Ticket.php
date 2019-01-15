<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use App\Location;
use App\User;

class Ticket extends Model
{
    public function location()
    {
        //$ticket = App\Ticket::find(1);
        //echo $ticket->location->rate
        return $this->belongsTo(Location::class);
    }

    public function user() {
        return $this->belongsTo(User::class, 'owner_id');
    }
}
