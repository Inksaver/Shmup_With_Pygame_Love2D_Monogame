# https://www.youtube.com/watch?v=-5GNbL33hz0
'''
Only 1 player so use code module (static class) for player
and a separate shared code module for global variables
'''
# import libraries
import pygame, os
import shared, player, mob

mobs:list = []

def process_events() -> (object, object, bool):
	''' get keyboard input and check if user closed window '''
	key_down = None
	quit = False
	for event in pygame.event.get():				# process events
		if event.type == pygame.QUIT:				# close button clicked
			quit = True
		elif event.type == pygame.KEYDOWN:			# single press of any key
			key_down = event.key	

	keystate = pygame.key.get_pressed()				# get keypressed events
	if keystate[pygame.K_ESCAPE]:					# player pressing escape continuously
		quit = True

	return keystate, key_down, quit					# usage: if key_down == pygame.K_RETURN:

def check_mob_player_collisions(keystate:object, dt:float) -> None:
	''' update player and all mobs '''
	player.update(keystate, dt)
	for mob in mobs:
		mob.update(dt)

def load() -> None:
	''' Setup pygame and load all assets '''
	shared.game_folder = os.getcwd() 					# current directory
	os.environ["SDL_VIDEO_CENTERED"] = "1" 				# Centre the Pygame window on screen
	pygame.init()       								# initialise pygame and game window
	try:
		pygame.mixer.init() 							# start pygame sound library
	except:
		shared.audio_present = False					# audio driver not installed
	shared.screen = pygame.display.set_mode((shared.WIDTH, shared.HEIGHT)) 
	pygame.display.set_caption(shared.window_title) 	# The window title
	shared.clock = pygame.time.Clock() 					# Keep track of framerate etc

	player.init(40, 50, 500, shared.GREEN)				# create a player (50x40 green rectangle)

	for i in range(80):									# make 8 mobs
		mobs.append(mob.Mob(30, 40, shared.RED))

def update() -> None:
	'''
	Update all game items and keyboard input
	delta-time used in Love2D and Monogame is measured in seconds
	Can be obtained from clock.tick() in ms
	'''
	dt = shared.clock.tick(shared.FPS) / 1000				# update clock. dt can be passed to other update functions
	keystate, key_down, quit = process_events()				# get keypressed, keydown and close/esc user choice
	if quit:
		shared.gamestate = shared.gamestates["quit"]		# set gamestate to quit
	else:
		if shared.gamestate == shared.gamestates["play"]:
			check_mob_player_collisions(keystate, dt)		# update player and mobs

def draw() -> None:
	''' Draw background and all active sprites '''
	shared.screen.fill(shared.BLACK)					# make screen black
	if shared.gamestate == shared.gamestates["play"]:
		player.draw()
		for mob in mobs:
			mob.draw()		

	pygame.display.flip()	

def main() -> None:
	''' Run game loop and call other functions from here '''
	shared.WIDTH = 480									# default screen width: alter as required
	shared.HEIGHT = 600									# default screen height: alter as required
	shared.window_title = "Shmup!"						# default window title: change as required	
	load()												# setup window and game assets

	''' gameloop '''
	shared.gamestate = shared.gamestates["play"]		# gamestate starting at 1 ('play': no menu)
	while shared.gamestate < shared.gamestates["quit"]:	# 3 = quit
		update()										# go through update functions
		draw()											# go through draw functions

	pygame.quit()
	
main()
