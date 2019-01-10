<?php

use Illuminate\Database\Seeder;
use Faker\Generator as Faker;

class CompaniesTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        // $faker = Faker::create();
        DB::table('companies')->insert([
            'name' => 'Whoo Inc',
        ]);
    }
}
