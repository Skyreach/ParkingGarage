<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Company extends Model
{
    public function location() {
        return $this->hasMany(Location::class, 'company_id');
    }
}
