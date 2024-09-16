const PROXY_CONFIG = [
  {
    context: [
      "/",
    ],
    target: "https://localhost:7052",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
