<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreateLocationsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('locations', function (Blueprint $table) {
            $table->increments('id');
            $table->unsignedInteger('company_id');
            
            $table->string('name');
            $table->string('address');                                          //obviously can be broken up
            $table->decimal('rate')->default('3.00');                           // The amount to charge as the base rate
            $table->decimal('rate_increase_percentage')->default('1.00');       // Assumed that increases are by a static percentage
            $table->string('rate_durations')->default('60,180,360,1440');       // Minutes elapsed before rate increase applies
            $table->unsignedInteger('grace_period_seconds')->default(0);
            $table->timestamps();

            $table->foreign('company_id')->references('id')->on('company');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('locations');
    }
}
