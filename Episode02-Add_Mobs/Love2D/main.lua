-- Lua / Love2D equivalent of Python / Pygame Shmup Episode 2:
-- https://www.youtube.com/watch?v=-5GNbL33hz0

local Shared 	= require "shared"
local mobList	= {}	-- store all meteors (Mobs) in this table
local Player			-- declare Player local to this file
local Mob 				-- declare the Mob class

function processEvents()
	--[[get keyboard input and check if user closed window]]
	quit = false
	keyboardState = love.keyboard
	if keyboardState.isDown('escape') then
		quit = true
	end
	
	return keyboardState, quit
end

function love.load()
	if arg[#arg] == "-debug" then
		print("running in debug mode")
		require("mobdebug").start()
	end
	print("To prevent this console window opening, delete the line: 't.console = true' in the conf.lua file")	
	Player = require "player"		-- import Player static class
	Mob = require "mob"				-- import the Mob class
	
	Player.setProperties(50, 40, 500)	-- no constructor on static class so provide width, height and speed
	
	-- make 8 Mobs (Dangerous rectangles!) and store them in a list
	for i = 1, 8 do
		table.insert(mobList, Mob(30,40)) -- This one-liner creates a new Mob object and adds it to the mobList
	end
	Shared.gamestate = Shared.gamestates['play']
end

function love.update(dt)
	keyboardState, quit = processEvents()
	if quit or Shared.gamestate == Shared.gamestates["quit"] then
		love.event.quit()
	else
		if Shared.gamestate == Shared.gamestates["play"] then
			Player.update(dt, keyboardState)
		end
		for i = #mobList, 1, -1 do		-- update all Mobs						
			mobList[i]:update(dt)		-- note colon :update
		end
	end
end

function love.draw()
	Player.draw()					-- draw Player. Note single dot .draw
	
	love.graphics.setColor(Shared.RED)		-- change colour to red ready for any Mobs
	for i = 1, #mobList do
		mobList[i]:draw()			-- note colon :draw
	end
end