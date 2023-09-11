
/**keys links to different js files at different phase of process
 * during dev process keys links to dev, during production phase
 * keys links to prod
 */

if(process.env.NODE_ENV === 'production'){
    module.exports = require('./prod.js');
}else{
    module.exports = require('./dev.js');
}