# NETWORK_PROGRAMING_SERVER
Repo lưu mã nguồn bài tập hàng tuần (Server) - Ngô Trung Kiên - 74458 | Lập trình mạng | Kì 2 | 19.20 

CHANGELOG (17.5.2020)
1. Fix lỗi giới hạn kết nối 
2. Thêm chức năng : Khi client gửi 'dig domain.com', server trả về IP của domain đó 
3. Hiển IP:Port của Server ngay khi Server vừa mở lên 
4. Truyền giá trị MAX_CONNECTION ngay từ đối dòng lệnh
5. Hiển thị thông tin địa chỉ, port của client trên server mỗi khi có client mới kết nối đến
6. Thêm chức năng '429 Too Many Request'
7. Fix chức năng ghi file log text

ERROR : 
1. Server từ chối phục vụ Client đầu tiên, sau đó phục vụ tiếp các client thỏa mãn MAX_CONNECTION, từ chối các client không thỏa mãn MAX_CONNECTION 
