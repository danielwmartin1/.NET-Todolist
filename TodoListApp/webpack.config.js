const webpack = require('webpack');
const dotenv = require('dotenv');

dotenv.config();

module.exports = {
  // ...existing code...
  plugins: [
    new webpack.DefinePlugin({
      'process.env.SUPABASE_URL': JSON.stringify(process.env.SUPABASE_URL),
      'process.env.SUPABASE_KEY': JSON.stringify(process.env.SUPABASE_KEY),
    }),
    // ...existing code...
  ],
  // ...existing code...
};