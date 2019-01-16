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
        $vehicles = auth()->user()->vehicle;
        return view('vehicles.index', [
            'vehicle' => $vehicle
        ]);
    }

    public function create()
    {
        return view('vehicles.create');
    }

    public function store(Vehicle $vehicle)
    {
        // $this->authorize('update', $vehicle);
        $attr = request()->validate([
            'license_plate' => 'required|min:2|max:8',
            'name' => 'required',
        ]);
        
        auth()->user()->vehicle()->create($attr);
        return redirect('/tickets');
    }
    
    //todo: edit, update, destroy
}
