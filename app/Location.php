<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Location extends Model
{
    public function ticket() {
        return $this->hasmany(Ticket::class, 'location_id');
    }
}
