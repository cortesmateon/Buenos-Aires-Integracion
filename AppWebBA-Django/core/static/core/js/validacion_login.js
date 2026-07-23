// Ensure jQuery and jQuery Validation Plugin are loaded before this script
// This script should be placed inside a DOMContentLoaded listener or at the end of the body
// to ensure the form elements exist.

$(document).ready(function () { // Use jQuery's ready function for DOM readiness

    // --- Custom Validation Methods (can be defined once and reused) ---

    // Validates basic email format (something@something.something)
    $.validator.addMethod(
        "validateemail",
        function (value, element) { // No need for 'validate' parameter here if always true
            var re = /\S+@\S+\.\S+/;
            return re.test(value);
        },
        "Formato de correo incorrecto. Debe contener '@' y '.'."
    );

    // Validates if password contains at least one number
    $.validator.addMethod(
        "onenumber",
        function (value, element) {
            var re = new RegExp('.*[0-9].*');
            return re.test(value);
        },
        "La contraseña debe contener al menos un número."
    );

    // Validates if password contains at least one uppercase letter
    $.validator.addMethod(
        "onemayus",
        function (value, element) {
            var re = new RegExp('.*[A-Z].*');
            return re.test(value);
        },
        "La contraseña debe contener al menos una mayúscula."
    );

    // --- Apply Validation to the Login Form ---
    $('#formulario').validate({ // Target your form by its ID
        rules: {
            // Use the 'name' attribute of your input fields here
            "username": { // Corresponds to the 'username' field in IniciarSesionForm (your email input)
                required: true,
                validateemail: true // Apply custom email format validation
            },
            "password": { // Corresponds to the 'password' field in IniciarSesionForm
                required: true,
                // Consider if you really need these for LOGIN; usually they're for REGISTRATION
                // onenumber: true,
                // onemayus: true
            },
        },
        messages: {
            "username": {
                required: "El correo es un campo obligatorio.",
                validateemail: "Por favor, ingresa una dirección de correo válida."
            },
            "password": {
                required: "La contraseña es un campo obligatorio."
            }
        },
        errorElement: 'span', // Use <span> for error messages
        errorClass: 'client-validation-error-message', // Apply a specific class for styling
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid').removeClass('is-valid'); // Add Bootstrap's is-invalid class
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid').addClass('is-valid'); // Remove is-invalid
        },
        errorPlacement: function (error, element) {
            // Place error messages directly after the input field
            error.insertAfter(element);
        }
    });

    // You don't need these specific .rules("add") calls
    // because you've already defined them within the .validate() configuration above.
    // $("#id_username").rules("add", { validateemail: true }); // This would be redundant
    // $("#id_password").rules("add", { onenumber: true });    // These are also in the config
    // $("#id_password").rules("add", { onemayus: true });     // So, remove these lines.

});