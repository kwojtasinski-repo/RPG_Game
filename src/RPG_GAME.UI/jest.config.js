const esModules = ['lodash-es', 'axios'].join('|') // Problem -> SyntaxError: Cannot use import statement outside a module

module.exports = {
  preset: '@vue/cli-plugin-unit-jest',
  transform: { // Problem -> SyntaxError: Cannot use import statement outside a module
    [`^(${esModules}).+\\.js$`]: 'babel-jest',
  },
  transformIgnorePatterns: [`node_modules/(?!(${esModules}))`], // Problem -> SyntaxError: Cannot use import statement outside a module
}
