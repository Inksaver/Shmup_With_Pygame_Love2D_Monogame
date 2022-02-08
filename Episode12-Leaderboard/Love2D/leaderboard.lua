--[[
The Monogame and Pygame versions needed a sub-class to create the 
scoreData objects. In Lua this is achieved with a simple table which
holds a list of other tables.
Each of the sub-tables holds a name/score pair:
scoreList =
{
	{name = "FRED", score = 1234},
	{name = "INKSAVER", score = 3234}
}
]]

local Class = require("lib.Class")
local Leaderboard = Class:derive("Leaderboard")
local Rectangle = require "lib.Rectangle"
local Shared = require "shared"
require "lib/Utils" -- for split function
	
function Leaderboard:addScoreData(line)
	--[[ either "FRED;356" or {"FRED", 356} ]]
	local data
	-- line could be a string or table. Lua/Python functions cannot have overloads
	-- so this method is the closest you can get
	if type(line) == "string" then
		data = line:upper():split(';')
	else -- table
		data = line
	end
	-- create a local table of name/score pairs
	local newScore = {}
	newScore.name = data[1]
	newScore.score = tonumber(data[2])
	local insertAt = #self.scoreList + 1 -- assume end of list
	-- calculate insert position high -> low
	for i = 1, #self.scoreList do --needs at least 1 item
		if newScore.score >= self.scoreList[i].score then 
			insertAt = i
			break
		end
	end
	-- insert the newScore table into scoreList
	table.insert(self.scoreList, insertAt, newScore)
end

function Leaderboard:new()
	--[[ class constructor, no parameters]]
	self.currentPlayerEntered = false
	self.scoreList = {} 		-- list of name/score sub-tables
	self.name = ""				-- get player input and store here
	self.ypos = 80
	self:populateScoreList()	-- read existing highscore text file
end

function Leaderboard:populateScoreList()
	--[[
	You have no choice where to save files. Default folder used:
	C:\Users\<username>\AppData\Roaming\LOVE\ShmupHighscore\Highscore.txt
	]]
	self.scoreList = {} -- clear the scoreList
	for line in love.filesystem.lines("Highscore.txt") do
		self:addScoreData(line)
	end
end

function Leaderboard:addEntry()
	if self.name ~= "" then
		self.currentPlayerEntered = true
		self:addScoreData({self.name, Shared.score})
		Shared.inputText = ""
		self:writeScoreList()
		self:populateScoreList()
	end
end

function Leaderboard:writeScoreList()
	--[[ C:\Users\<username>\AppData\Roaming\LOVE\ShmupHighscore\Highscore.txt
		self.scoreList = {'FRED;200','JOHN;450','MARY;286'}
	]]
	-- write the top 5 scoring values
	local text = ""
	endloop = #self.scoreList
	if endloop > 5 then endloop = 5 end
	for line = 1, endloop do
		local data = self.scoreList[line]
		text = text .. data.name .. ";".. data.score .. "\n"
	end
	success, errormsg = love.filesystem.write("Highscore.txt", text)
end

function Leaderboard:update(keyboard)
	--has Name been input?
	if self.currentPlayerEntered then -- name already obtained. waiting for c to continue
		if keyboard.isDown("c") then
			Shared.gamestate = Shared.gamestates['menu']
			self.currentPlayerEntered = false
			Shared.inputText = ""
		end
	else	-- If not get input from user
		self.name = Shared.inputText
	end
end

function Leaderboard:draw()
	Shared.drawText("LEADERBOARD", 'size50', 0, 0, "centre", Shared.YELLOW)			-- 'Leaderboard'
	-- draw a list of 5 names with current name in position related to score
	self.ypos = 80
	local lines = 0
	for _,value in ipairs(self.scoreList) do		-- up to 5 highscore names and scores
		Shared.drawText(value.name.." : "..value.score, 'size24', 0, self.ypos, "centre", Shared.WHITE)
		self.ypos = self.ypos + 30
		lines = lines + 1
		if lines > 5 then
			break
		end
	end
	
	if self.currentPlayerEntered then -- If got input from user
		Shared.drawText("Press C to continue", 'size18', 0, Shared.HEIGHT * 0.8, "centre", Shared.YELLOW)
	else
		Shared.drawText("Type your name and press Enter", 'size18', 0, Shared.HEIGHT * 0.8, "centre", Shared.WHITE)
		--displayBox(text, textSize, rect, foreColour, backColour, textColour)
		Shared.displayBox("NAME: ".. self.name, 'size18', Rectangle(50, Shared.HEIGHT * 0.85, Shared.WIDTH - 100, 24), Shared.YELLOW, Shared.DARKBLUE, Shared.WHITE)
	end
end

return Leaderboard
