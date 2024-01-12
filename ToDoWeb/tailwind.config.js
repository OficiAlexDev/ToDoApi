/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        primary:"#1342FA",
        primaryLight:"#206BFF",
        primaryDark:"#061860",
      },
    },
  },
  plugins: [],
};
