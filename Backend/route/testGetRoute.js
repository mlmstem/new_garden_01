const mongoose = require('mongoose');
const Account = mongoose.model('accounts');

module.exports = app => {
    app.get('/getData', async (req, res) => {
        const { username } = req.body;

        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User not found" });
            return;
        }

        res.send(userAccount.username);
    });
}