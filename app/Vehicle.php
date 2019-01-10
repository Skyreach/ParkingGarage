<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Vehicle extends Model
{
    public $fillable = [
        'license_plate',
        'name',
    ];
}
