--generic class table
local Class = {}
Class.__index = Class --metamethod to index itself
function Class:new() end

-- create new class type 
function Class:derive(classType)
	assert(classType ~= nil, "parameter classType must not be nil")
	assert(type(classType) == "string", "parameter classType must be string")
	local cls = {}
	cls["__call"] = Class.__call
	cls.__index = cls
	cls.super = self
	setmetatable(cls, self) -- allows inheritance
	return cls
end

-- allow table to be treated as a function
function Class:__call(...)
	local inst = setmetatable({}, self) --create instance of Class
	inst:new(...)
	return inst
end

return Class
