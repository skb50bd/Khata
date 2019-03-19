# Create User
net user ServiceUser KhataUser /add

# Grant Permission
icacls "D:\Khata\Service" /grant "ServiceUser:(OI)(CI)WRX /t"

# Register Service
.\RegisterService.ps1 
    -Name Khata 
    -DisplayName "Khata - Service" 
    -Description "Khata as a Windows Service." 
    -Path "c:\svc" 
    -Exe Khata.exe
    -User ROG\ServiceUser
