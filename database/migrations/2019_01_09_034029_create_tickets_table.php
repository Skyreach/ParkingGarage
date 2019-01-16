<?php

use Illuminate\Support\Facades\Schema;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Database\Migrations\Migration;

class CreateTicketsTable extends Migration
{
    /**
     * Run the migrations.
     *
     * @return void
     */
    public function up()
    {
        Schema::create('tickets', function (Blueprint $table) {
            $table->increments('id');
            $table->unsignedInteger('owner_id');
            $table->unsignedInteger('location_id');
            $table->dateTime('datetime_issued')->default(DB::raw('now()'));    // consider renewals if paying in advance
            $table->boolean('is_active')->default(true);
            $table->boolean('is_paid')->default(false);
            $table->timestamps();

        });
        
        Schema::table('tickets', function(Blueprint $table) {
            $table->foreign('owner_id')->references('id')->on('users');
            $table->foreign('location_id')->references('id')->on('locations');
        });
    }

    /**
     * Reverse the migrations.
     *
     * @return void
     */
    public function down()
    {
        Schema::dropIfExists('tickets');
    }
}
