<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Location extends Model
{
    public function ticket() {
        return $this->hasmany(Ticket::class, 'location_id');
    }

    public function company() {
        return $this->belongsTo(Company::class, 'company_id');
    }
}
