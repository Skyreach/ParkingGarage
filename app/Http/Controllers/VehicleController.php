<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Vehicle;

class VehicleController extends Controller
{
    public function __construct()
    {
        $this->middleware('auth');
    }

    public function index()
    {
        $vehicles = auth()->user()->vehicles;
        return view('vehicles.index', $vehicles);
    }

    public function create()
    {
        return view('vehicles.create');
    }

    public function store(Vehicle $vehicle)
    {
        // $this->authorize('update', $vehicle);
        $attr = request()->validate([
            'licence_plate' => ['required', 'min:2', 'max:8'],
        ]);
        
        auth()->user()->vehicle()->create($attr);
    }
    
    //todo: edit, update, destroy
}
