-- Lua / Love2D equivalent of Python / Pygame Shmup Episode 1:
-- https://www.youtube.com/watch?v=nGufy7weyGY
local Shared = require "shared"
local Player	-- declare Player local to this file

local function processEvents()
	--[[get keyboard input and check if user closed window]]
	local quit = false
	local keyboardState = love.keyboard
	if keyboardState.isDown('escape') then
		quit = true
	end
	
	return keyboardState, quit
end

function love.load()
	--[[
	load function runs once on loading. Use to initialise variables
	
	in ZeroBrane, using single green triangle or F5 or Project-> Start Debugging adds -debug to the command line.
	This can be used to print out data to help debugging.
	If working ok the line "running in debug mode" appears in the console
	]]
	if arg[#arg] == "-debug" then
		print("running in debug mode")
		require("mobdebug").start()
	end
	print("To prevent this console window opening, delete the line: 't.console = true' in the conf.lua file")	
	Player = require "player"			-- import Player static class
	Player.setProperties(40, 50, 500)	-- no constructor on static class so provide width, height and speed
	Shared.gamestate = Shared.gamestates['play']
	--Shared.debug = false --no effect until episode 4
end

function love.update(dt)
	local keyboardState, quit = processEvents()
	if quit or Shared.gamestate == Shared.gamestates["quit"] then
		love.event.quit()
	else
		if Shared.gamestate == Shared.gamestates["play"] then
			Player.update(keyboardState, dt) -- update Player
		end
	end	
end

function love.draw()				
	Player.draw()	-- draw Player
end
