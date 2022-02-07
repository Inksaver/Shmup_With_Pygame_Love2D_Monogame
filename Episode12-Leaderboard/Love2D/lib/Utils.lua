function string:split(separator)
	--[[use: outputTable = some;random;strings:split(';') or tblSplit = string.split(some;random;strings, ';')]]   
	local tempTable = {}
	local returnTable = {}

	if self:len() > 0 then
		local field, start = 1, 1
		local first, last = self:find(separator, start)
		while first do
			tempTable[field] = self:sub(start, first - 1)
			field = field + 1
			start = last + 1
			first, last = self:find(separator, start)
		end
		tempTable[field] = self:sub(start)
		returnTable = tempTable
	end
	
	return returnTable
end

function shallowCopy(original)
	local copy = {}
	for key, value in pairs(original) do
		copy[key] = value
	end
	return copy
end