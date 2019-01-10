<?php

use Illuminate\Database\Seeder;
use Faker\Generator;
use App\Company;

class LocationsTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        // $faker = Faker::create();
        $company = Company::first();
        DB::table('locations')->insert([
            'lot_name' => 'Lot1',
            'company_id' => $company->id,
            'address' => '123 Fake Street',
            'occupancy' => '50',
        ]);
    }
}
