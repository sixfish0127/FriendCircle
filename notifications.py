from flask import Flask, request, jsonify

app = Flask(__name__)

@app.route('/api/process-notifications', methods=['POST'])
def receive_notification():
    try:
        # 接收C#的JSON數據
        data = request.json
        if not data:            
            return jsonify({"status": "error", "message": "No data received"}), 400
        # 提取通知數據
        notification_id = data.get("id")
        user_id = data.get("userId")
        comment_id = data.get("commentId")
        message = data.get("message")
        created_at = data.get("createdAt")
        is_read = data.get("isRead", False)
        
        # 在控制台打印接收到的數據
        print("Received Notification:")
        print(f"ID: {notification_id}")
        print(f"User ID: {user_id}")
        print(f"Comment ID: {comment_id}")
        print(f"Message: {message}")
        print(f"Created At: {created_at}")
        print(f"Is Read: {is_read}")
        
        return jsonify({"status": "success", "message": "Notification processed successfully"})    
    
    except Exception as e:
    # 捕獲並返回錯誤信息
       return jsonify({"status": "error", "message": str(e)}), 500
if __name__ == '__main__':
    app.run(port=7281)
