/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{html,js,cshtml}",
        "./Areas/**/*.{html,js,cshtml}",
        "./wwwroot/js/**/*.js"
    ],
    theme: {
        extend: {
            colors: {
                'accent': { 
                    300: '#FCE488',
                    500: '#EACF69',
                    700: '#ad9333'
                },
            },
        },
    },
    plugins: [],
}
