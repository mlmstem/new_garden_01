
from flask import Flask, jsonify
import subprocess

app = Flask(__name__)

@app.route('/runscript')
def run_script():
    try:
        result = subprocess.run([r"E:\FarmBotFlask\venv\Scripts\python", "Observer.py"], check=True, capture_output=True, text=True, timeout=600)
        return jsonify({"status": "success", "output": result.stdout})
    except subprocess.CalledProcessError as e:
        return jsonify({"status": "error", "error": e.stderr})
    except subprocess.TimeoutExpired as e:
        return jsonify({"status": "timeout", "output": e.stdout, "error": e.stderr})

if __name__ == '__main__':
    app.run(debug=True)
