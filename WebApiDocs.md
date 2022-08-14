# AdPang-s-Graduation-Project
adPang的毕业设计-FilesManagerSystem
# Admin FileManagent System API


**简介**:Admin FileManagent System API


**HOST**:


**联系人**:


**Version**:v1


**接口路径**:/v1/admin-filemanager-api-docs


[TOC]






# Oauth


## 用户登录 （用户名密码登录）


**接口地址**:`/api/Oauth/Auth`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "userName": "",
  "email": "",
  "password": "",
  "phoneNumber": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userDto|UserDto|body|true|UserDto|UserDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;userName|||false|string||
|&emsp;&emsp;email|||false|string||
|&emsp;&emsp;password|||false|string||
|&emsp;&emsp;phoneNumber|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 用户注册


**接口地址**:`/api/Oauth/Register`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "userName": "",
  "email": "",
  "password": "",
  "phoneNumber": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userDto|UserDto|body|true|UserDto|UserDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;userName|||false|string||
|&emsp;&emsp;email|||false|string||
|&emsp;&emsp;password|||false|string||
|&emsp;&emsp;phoneNumber|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# RoleManage


## 获取角色列表 (Auth roles: Admin)


**接口地址**:`/api/RoleManage/GetRoles/Gets/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|RoleDtoPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||RoleDtoPagedList|RoleDtoPagedList|
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;items||array|RoleDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;name||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"pageIndex": 0,
		"pageSize": 0,
		"totalCount": 0,
		"totalPages": 0,
		"indexFrom": 0,
		"items": [
			{
				"id": "",
				"name": ""
			}
		]
	}
}
```


## 根据userId获取对应的角色列表 (Auth roles: Admin)


**接口地址**:`/api/RoleManage/GetRolesByUserId/GetRoles/{userId}/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId|用户Id|path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringIListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": []
}
```


## 获取当前请求人的角色 (Auth)


**接口地址**:`/api/RoleManage/GetRolesASync/GetRoles`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


暂无


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringIListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": []
}
```


## 添加角色 (Auth roles: Admin)


**接口地址**:`/api/RoleManage/AddRole/add/admin`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "name": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|roleDto|RoleDto|body|true|RoleDto|RoleDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;name|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": ""
}
```


## 修改角色信息 (Auth roles: Admin)


**接口地址**:`/api/RoleManage/EditRole/Edit/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "name": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|roleDto|RoleDto|body|true|RoleDto|RoleDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;name|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除角色 (Auth roles: Admin)


**接口地址**:`/api/RoleManage/DeleteRole/Delete/{roleId}/admin`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|roleId|角色Id|path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# UserManage


## 获取当前用户Id (Auth roles: Ordinary)


**接口地址**:`/api/UserManage/GetUserId`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


暂无


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success||
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


暂无


**响应示例**:
```javascript

```


## 获得用户列表 (Auth roles: Admin)


**接口地址**:`/api/UserManage/GetUsers/GetAll/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserDtoPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||UserDtoPagedList|UserDtoPagedList|
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;items||array|UserDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;userName||string||
|&emsp;&emsp;&emsp;&emsp;email||string||
|&emsp;&emsp;&emsp;&emsp;password||string||
|&emsp;&emsp;&emsp;&emsp;phoneNumber||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"pageIndex": 0,
		"pageSize": 0,
		"totalCount": 0,
		"totalPages": 0,
		"indexFrom": 0,
		"items": [
			{
				"id": "",
				"userName": "",
				"email": "",
				"password": "",
				"phoneNumber": ""
			}
		]
	}
}
```


## 根据角色名获取用户列表 (Auth roles: Admin)


**接口地址**:`/api/UserManage/GetUsersByRole/Get/{roleName}/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|roleName||path|true|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserDtoListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array|UserDto|
|&emsp;&emsp;id||string(uuid)||
|&emsp;&emsp;userName||string||
|&emsp;&emsp;email||string||
|&emsp;&emsp;password||string||
|&emsp;&emsp;phoneNumber||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": [
		{
			"id": "",
			"userName": "",
			"email": "",
			"password": "",
			"phoneNumber": ""
		}
	]
}
```


## 给用户添加角色 (Auth roles: Admin)


**接口地址**:`/api/UserManage/AddUserToRole/Add/{userId}/{roleName}/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId|用户Id|path|true|string(uuid)||
|roleName|角色名|path|true|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": ""
}
```


## 给用户添加角色 (Auth roles: Admin)


**接口地址**:`/api/UserManage/AddUsersToRole/Adds/{userId}/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
[]
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId|用户Id|path|true|string(uuid)||
|strings|string|body|true|array||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringIListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": []
}
```


## 删除用户的角色 (Auth roles: Admin)


**接口地址**:`/api/UserManage/RemoveRoleFormUser/Remove/{userId}/{roleName}/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId|用户Id|path|true|string(uuid)||
|roleName|角色名|path|true|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": ""
}
```


## 删除用户的角色（多个角色） (Auth roles: Admin)


**接口地址**:`/api/UserManage/RemoveRolesFormUser/Rmoves/{userId}/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
[]
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId|用户Id|path|true|string(uuid)||
|strings|string|body|true|array||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringIListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": []
}
```


# VerfiyCode


## 获取图片验证码


**接口地址**:`/api/VerfiyCode/GetGraphic4VerfiyCode`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|seed||query|false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success||


**响应参数**:


暂无


**响应示例**:
```javascript

```


## 获取邮箱验证码


**接口地址**:`/api/VerfiyCode/GetMailVerfiyCode`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|email|邮箱账号|query|false|string||
|operaType|操作类型|query|false|MailMsgOperaType|MailMsgOperaType|
|&emsp;&emsp;undefined|可用值:0,1,2,3||false|integer||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# PrivateDisk


## 获取当前用户下所有的硬盘列表 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateDisk/GetDiskListFromUser/GetAll`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|PrivateDiskInfoDtoPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||PrivateDiskInfoDtoPagedList|PrivateDiskInfoDtoPagedList|
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;items||array|PrivateDiskInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;diskName||string||
|&emsp;&emsp;&emsp;&emsp;diskSN||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"pageIndex": 0,
		"pageSize": 0,
		"totalCount": 0,
		"totalPages": 0,
		"indexFrom": 0,
		"items": [
			{
				"id": "",
				"diskName": "",
				"diskSN": ""
			}
		]
	}
}
```


## 添加硬盘 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateDisk/AddDisk/Add`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "diskName": "",
  "diskSN": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|privateDiskInfoDto|PrivateDiskInfoDto|body|true|PrivateDiskInfoDto|PrivateDiskInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;diskName|||false|string||
|&emsp;&emsp;diskSN|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 修改硬盘信息 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateDisk/EditDisk/Edit`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "diskName": "",
  "diskSN": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|privateDiskInfoDto|PrivateDiskInfoDto|body|true|PrivateDiskInfoDto|PrivateDiskInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;diskName|||false|string||
|&emsp;&emsp;diskSN|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除硬盘信息 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateDisk/DeleteDisk/Delete/{diskId}`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|diskId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除硬盘信息（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/DeleteDisk/Delete/{diskId}/{userId}/admin`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|diskId||path|true|string(uuid)||
|userId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 添加硬盘信息（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/AddDisk/Add/{userId}/admin`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "diskName": "",
  "diskSN": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||
|privateDiskInfoDto|PrivateDiskInfoDto|body|true|PrivateDiskInfoDto|PrivateDiskInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;diskName|||false|string||
|&emsp;&emsp;diskSN|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 修改硬盘信息（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/EditDisk/Edit/{userId}/admin`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "diskName": "",
  "diskSN": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||
|privateDiskInfoDto|PrivateDiskInfoDto|body|true|PrivateDiskInfoDto|PrivateDiskInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;diskName|||false|string||
|&emsp;&emsp;diskSN|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取所有硬盘信息，包括文件（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/GetDiskList/GetAll/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|DiskInfoContainFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||DiskInfoContainFileInfoDtoIPagedList|DiskInfoContainFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|DiskInfoContainFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;diskName||string||
|&emsp;&emsp;&emsp;&emsp;diskSN||string||
|&emsp;&emsp;&emsp;&emsp;privateFiles||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;diskId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取指定用户的所有硬盘，包括文件（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/GetDiskDetail/GetDiskDetial/{userId}/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserInfoContainDiskInfoDtoApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||UserInfoContainDiskInfoDto|UserInfoContainDiskInfoDto|
|&emsp;&emsp;id||string(uuid)||
|&emsp;&emsp;userName||string||
|&emsp;&emsp;email||string||
|&emsp;&emsp;privateDiskInfos||array|DiskInfoContainFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;diskName||string||
|&emsp;&emsp;&emsp;&emsp;diskSN||string||
|&emsp;&emsp;&emsp;&emsp;privateFiles||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;diskId||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"id": "",
		"userName": "",
		"email": "",
		"privateDiskInfos": [
			{
				"id": "",
				"diskName": "",
				"diskSN": "",
				"privateFiles": [
					{
						"id": "",
						"fileName": "",
						"fileType": "",
						"fileLength": 0,
						"filePath": "",
						"fileMD5Str": "",
						"diskId": ""
					}
				]
			}
		]
	}
}
```


## 获取所有用户的所有硬盘，包括文件（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateDisk/GetDiskDetail/GetAllDiskDetail/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserInfoContainDiskInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||UserInfoContainDiskInfoDtoIPagedList|UserInfoContainDiskInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|UserInfoContainDiskInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;userName||string||
|&emsp;&emsp;&emsp;&emsp;email||string||
|&emsp;&emsp;&emsp;&emsp;privateDiskInfos||array|DiskInfoContainFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;diskName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;diskSN||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;privateFiles||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;diskId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# PrivateFile


## 获取当前账号的文件详情列表 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/GetFileInfos/GetAll`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|PrivateFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||PrivateFileInfoDtoIPagedList|PrivateFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;diskId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 根据文件id获取文件详情 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/GetFileInfo/Get/{fileId}`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|fileId|文件ID|path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|PrivateFileInfoDtoApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||PrivateFileInfoDto|PrivateFileInfoDto|
|&emsp;&emsp;id||string(uuid)||
|&emsp;&emsp;fileName||string||
|&emsp;&emsp;fileType||string||
|&emsp;&emsp;fileLength||integer(int64)||
|&emsp;&emsp;filePath||string||
|&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;diskId||string(uuid)||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"id": "",
		"fileName": "",
		"fileType": "",
		"fileLength": 0,
		"filePath": "",
		"fileMD5Str": "",
		"diskId": ""
	}
}
```


## 编辑文件 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/EditFileInfo/Edit`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "fileName": "",
  "fileType": "",
  "fileLength": 0,
  "filePath": "",
  "fileMD5Str": "",
  "diskId": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|privateFileInfoDto|PrivateFileInfoDto|body|true|PrivateFileInfoDto|PrivateFileInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;fileName|||false|string||
|&emsp;&emsp;fileType|||false|string||
|&emsp;&emsp;fileLength|||false|integer(int64)||
|&emsp;&emsp;filePath|||false|string||
|&emsp;&emsp;fileMD5Str|||false|string||
|&emsp;&emsp;diskId|||false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除文件 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/DeleteFileInfo/Delete/{fileId}`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|fileId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 添加文件信息 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/AddFileInfo/Add`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "fileName": "",
  "fileType": "",
  "fileLength": 0,
  "filePath": "",
  "fileMD5Str": "",
  "diskId": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|privateFileInfoDto|PrivateFileInfoDto|body|true|PrivateFileInfoDto|PrivateFileInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;fileName|||false|string||
|&emsp;&emsp;fileType|||false|string||
|&emsp;&emsp;fileLength|||false|integer(int64)||
|&emsp;&emsp;filePath|||false|string||
|&emsp;&emsp;fileMD5Str|||false|string||
|&emsp;&emsp;diskId|||false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 批量添加 (Auth roles: Ordinary)


**接口地址**:`/api/PrivateFile/AddFileInfos`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
[
  {
    "id": "",
    "fileName": "",
    "fileType": "",
    "fileLength": 0,
    "filePath": "",
    "fileMD5Str": "",
    "diskId": ""
  }
]
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|privateFileInfoDtos|PrivateFileInfoDto|body|true|array|PrivateFileInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;fileName|||false|string||
|&emsp;&emsp;fileType|||false|string||
|&emsp;&emsp;fileLength|||false|integer(int64)||
|&emsp;&emsp;filePath|||false|string||
|&emsp;&emsp;fileMD5Str|||false|string||
|&emsp;&emsp;diskId|||false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取指定用户的文件信息列表（管理员） (Auth roles: Admin)


**接口地址**:`/api/PrivateFile/GetFileInfos/GetAll/{userId}/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|PrivateFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||PrivateFileInfoDtoIPagedList|PrivateFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;diskId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取全部的文件信息列表 (Auth roles: Admin)


**接口地址**:`/api/PrivateFile/GetAllFileInfos/GetAll/admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|PrivateFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||PrivateFileInfoDtoIPagedList|PrivateFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|PrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;diskId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# DirInfo


## 获取当前账户的文件夹信息 (Auth roles: Ordinary)


**接口地址**:`/api/DirInfo/GetAllDir/Get`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|DirInfoDetailDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||DirInfoDetailDtoIPagedList|DirInfoDetailDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;dirName||string||
|&emsp;&emsp;&emsp;&emsp;childrenFileInfo||array|UserPrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;realFileInfo||CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;childrenDirInfo||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;parentDirInfoId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 添加文件夹信息 (Auth roles: Ordinary)


**接口地址**:`/api/DirInfo/AddDir/Add`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "dirName": "",
  "parentDirInfoId": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|dirInfoDto|DirInfoDto|body|true|DirInfoDto|DirInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;dirName|||false|string||
|&emsp;&emsp;parentDirInfoId|||false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除文件夹 (Auth roles: Ordinary)


**接口地址**:`/api/DirInfo/DeleteDir/Delete/{dirId}`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|dirId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 编辑 (Auth roles: Ordinary)


**接口地址**:`/api/DirInfo/EditDir/Edit`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "dirName": "",
  "parentDirInfoId": ""
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|dirInfoDto|DirInfoDto|body|true|DirInfoDto|DirInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;dirName|||false|string||
|&emsp;&emsp;parentDirInfoId|||false|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取指定用户的所有文件夹详情信息（包含层级关系）（管理员） (Auth roles: Admin)


**接口地址**:`/api/DirInfo/GetAllDir/Get/{userId}/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|DirInfoDetailDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||DirInfoDetailDtoIPagedList|DirInfoDetailDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;dirName||string||
|&emsp;&emsp;&emsp;&emsp;childrenFileInfo||array|UserPrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;realFileInfo||CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;childrenDirInfo||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;parentDirInfoId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取所有用户的文件夹列表（管理员） (Auth roles: Admin)


**接口地址**:`/api/DirInfo/GetAllUserDir/GetAll/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserInfoContainDirInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||UserInfoContainDirInfoDtoIPagedList|UserInfoContainDirInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|UserInfoContainDirInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;userName||string||
|&emsp;&emsp;&emsp;&emsp;email||string||
|&emsp;&emsp;&emsp;&emsp;dirInfos||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;dirName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;childrenFileInfo||array|UserPrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;realFileInfo||CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;childrenDirInfo||array|DirInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;parentDirInfoId||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# FileManage


## 文件上传 (Auth roles: Ordinary)


**接口地址**:`/api/FileManage/UploadFile/FileUpload`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,multipart/form-data`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|files||query|false|array|string|


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|StringStringKeyValuePairIEnumerableApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||array|StringStringKeyValuePair|
|&emsp;&emsp;key||string||
|&emsp;&emsp;value||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": [
		{
			"key": "",
			"value": ""
		}
	]
}
```


## 添加文件到云 (Auth roles: Ordinary)


**接口地址**:`/api/FileManage/AddFileToCloud/Add/{fileId}/{dirId}`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "fileName": "",
  "realFileInfo": {
    "id": "",
    "fileType": "",
    "fileLength": 0,
    "fileMD5Str": ""
  }
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|fileId||path|true|string(uuid)||
|dirId||path|true|string(uuid)||
|userPrivateFileInfoDto|UserPrivateFileInfoDto|body|true|UserPrivateFileInfoDto|UserPrivateFileInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;fileName|||false|string||
|&emsp;&emsp;realFileInfo|||false|CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id|||false|string||
|&emsp;&emsp;&emsp;&emsp;fileType|||false|string||
|&emsp;&emsp;&emsp;&emsp;fileLength|||false|integer||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 修改文件（文件名、文件所在文件夹） (Auth roles: Ordinary)


**接口地址**:`/api/FileManage/EditFileInfo/Edit/{dirId}`


**请求方式**:`PUT`


**请求数据类型**:`application/x-www-form-urlencoded,application/json-patch+json,application/json,text/json,application/*+json`


**响应数据类型**:`*/*`


**接口描述**:


**请求示例**:


```javascript
{
  "id": "",
  "fileName": "",
  "realFileInfo": {
    "id": "",
    "fileType": "",
    "fileLength": 0,
    "fileMD5Str": ""
  }
}
```


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|dirId||path|true|string(uuid)||
|userPrivateFileInfoDto|UserPrivateFileInfoDto|body|true|UserPrivateFileInfoDto|UserPrivateFileInfoDto|
|&emsp;&emsp;id|||false|string(uuid)||
|&emsp;&emsp;fileName|||false|string||
|&emsp;&emsp;realFileInfo|||false|CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id|||false|string||
|&emsp;&emsp;&emsp;&emsp;fileType|||false|string||
|&emsp;&emsp;&emsp;&emsp;fileLength|||false|integer||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str|||false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除文件 (Auth roles: Ordinary)


**接口地址**:`/api/FileManage/DeleteFileInfo/Delete/{fileId}`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|fileId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 下载云盘文件 (Auth roles: Ordinary)


**接口地址**:`/api/FileManage/DownloadFile/file/{fileId}`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|fileId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success||
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


暂无


**响应示例**:
```javascript

```


## 获取所有用户（包含用户文件信息） (Auth roles: Admin)


**接口地址**:`/api/FileManage/GetUserFileInfos/GetUsers/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|UserInfoContainCloudFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||UserInfoContainCloudFileInfoDtoIPagedList|UserInfoContainCloudFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|UserInfoContainCloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;userName||string||
|&emsp;&emsp;&emsp;&emsp;email||string||
|&emsp;&emsp;&emsp;&emsp;userPrivateFileInfos||array|UserPrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;realFileInfo||CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取所有已保存到云的文件 (Auth roles: Admin)


**接口地址**:`/api/FileManage/GetCloudFileInfos/GetAll/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|CloudFileInfoDetailDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||CloudFileInfoDetailDtoIPagedList|CloudFileInfoDetailDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|CloudFileInfoDetailDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;filePath||string||
|&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;&emsp;&emsp;users||array|UserDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;userName||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;email||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;password||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;phoneNumber||string||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 删除文件（管理员） (Auth roles: Admin)


**接口地址**:`/api/FileManage/DeleteFileInfo/Delete/{userId}/{fileId}/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|userId||path|true|string(uuid)||
|fileId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


# FileShared


## 创建分享 (Auth roles: Ordinary)


**接口地址**:`/api/FileShared/AddFileShared/Add`


**请求方式**:`POST`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|beSharedInfoId|分享Id|query|false|string(uuid)||
|isSingleFile|是否是单个文件分享|query|false|boolean||
|sharedPassword|分享密码|query|false|string||
|sharedDesc|文件分享描述|query|false|string||
|hasExpired|是否有过期时间|query|false|boolean||
|expiredTime|过期时间(天数)|query|false|integer(int32)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|SharedFileInfoDtoApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||SharedFileInfoDto|SharedFileInfoDto|
|&emsp;&emsp;id||string(uuid)||
|&emsp;&emsp;sharedDesc||string||
|&emsp;&emsp;sharedPassword||string||
|&emsp;&emsp;dirId||string(uuid)||
|&emsp;&emsp;singleFileId||string(uuid)||
|&emsp;&emsp;isSingleFile||boolean||
|&emsp;&emsp;hasExpired||boolean||
|&emsp;&emsp;expiredTime||string(date-time)||
|&emsp;&emsp;isExpired||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"id": "",
		"sharedDesc": "",
		"sharedPassword": "",
		"dirId": "",
		"singleFileId": "",
		"isSingleFile": true,
		"hasExpired": true,
		"expiredTime": ""
	}
}
```


## 删除分享信息 (Auth roles: Ordinary)


**接口地址**:`/api/FileShared/DeleteSharedInfo/Delete/{sharedId}`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|sharedId|分享信息Id|path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取文件信息 (Auth roles: Ordinary)


**接口地址**:`/api/FileShared/GetFileShared/Get/{sharedId}`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|sharedId|分享信息Id|path|true|string(uuid)||
|password|分享信息密码|query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|DirInfoDetailDtoApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||DirInfoDetailDto|DirInfoDetailDto|
|&emsp;&emsp;id||string(uuid)||
|&emsp;&emsp;dirName||string||
|&emsp;&emsp;childrenFileInfo||array|UserPrivateFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;fileName||string||
|&emsp;&emsp;&emsp;&emsp;realFileInfo||CloudFileInfoDto|CloudFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileType||string||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileLength||integer||
|&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;fileMD5Str||string||
|&emsp;&emsp;childrenDirInfo||array|DirInfoDetailDto|
|&emsp;&emsp;parentDirInfoId||string(uuid)||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {
		"id": "",
		"dirName": "",
		"childrenFileInfo": [
			{
				"id": "",
				"fileName": "",
				"realFileInfo": {
					"id": "",
					"fileType": "",
					"fileLength": 0,
					"fileMD5Str": ""
				}
			}
		],
		"childrenDirInfo": [
			{}
		],
		"parentDirInfoId": ""
	}
}
```


## 删除分享信息（管理员） (Auth roles: Admin)


**接口地址**:`/api/FileShared/DeleteSharedInfoAdmin/Delete/{sharedId}/Admin`


**请求方式**:`DELETE`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|sharedId||path|true|string(uuid)||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|ApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||string||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```


## 获取分享信息列表（管理员） (Auth roles: Admin)


**接口地址**:`/api/FileShared/GetSharedInfos/GetAll/Admin`


**请求方式**:`GET`


**请求数据类型**:`application/x-www-form-urlencoded`


**响应数据类型**:`*/*`


**接口描述**:


**请求参数**:


| 参数名称 | 参数说明 | 请求类型    | 是否必须 | 数据类型 | schema |
| -------- | -------- | ----- | -------- | -------- | ------ |
|PageIndex||query|false|integer(int32)||
|PageSize||query|false|integer(int32)||
|Search||query|false|string||


**响应状态**:


| 状态码 | 说明 | schema |
| -------- | -------- | ----- | 
|200|Success|SharedFileInfoDtoIPagedListApiResponse|
|401|Unauthorized||
|403|Forbidden||


**响应参数**:


| 参数名称 | 参数说明 | 类型 | schema |
| -------- | -------- | ----- |----- | 
|message||string||
|status||boolean||
|result||SharedFileInfoDtoIPagedList|SharedFileInfoDtoIPagedList|
|&emsp;&emsp;indexFrom||integer(int32)||
|&emsp;&emsp;pageIndex||integer(int32)||
|&emsp;&emsp;pageSize||integer(int32)||
|&emsp;&emsp;totalCount||integer(int32)||
|&emsp;&emsp;totalPages||integer(int32)||
|&emsp;&emsp;items||array|SharedFileInfoDto|
|&emsp;&emsp;&emsp;&emsp;id||string||
|&emsp;&emsp;&emsp;&emsp;sharedDesc||string||
|&emsp;&emsp;&emsp;&emsp;sharedPassword||string||
|&emsp;&emsp;&emsp;&emsp;dirId||string||
|&emsp;&emsp;&emsp;&emsp;singleFileId||string||
|&emsp;&emsp;&emsp;&emsp;isSingleFile||boolean||
|&emsp;&emsp;&emsp;&emsp;hasExpired||boolean||
|&emsp;&emsp;&emsp;&emsp;expiredTime||string||
|&emsp;&emsp;&emsp;&emsp;isExpired||boolean||
|&emsp;&emsp;hasPreviousPage||boolean||
|&emsp;&emsp;hasNextPage||boolean||


**响应示例**:
```javascript
{
	"message": "",
	"status": true,
	"result": {}
}
```
