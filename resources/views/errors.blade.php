@if ($errors->any())
    <ul>
        <div class="notification is-danger">
            @foreach ($errors->all() as $error)
                <li>{{ $error }}</li>
            @endforeach
        </div>
    </ul>
@endif