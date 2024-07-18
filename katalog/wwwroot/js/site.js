// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const search = document.querySelector('[name="search"]');

search.addEventListener('keyup', (e) => {
    if (e.code === 'Enter') {

        const url = '/?search=' + document.getElementById("searchinput").value;;

        window.location.href = url;
    }
});