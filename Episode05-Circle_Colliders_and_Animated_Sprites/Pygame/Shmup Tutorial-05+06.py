''' 
https://www.youtube.com/watch?v=_y5U8tB36Vk and https://www.youtube.com/watch?v=3Bk-Ny7WLzE
Add remaining images into img folder
Meteors will be rotated
Multiple meteor images are randomly chosen
Introduction of circle collider
Draw circles around images
'''
# import libraries
import pygame, os, math
import shared, player, mob, bullet

class data():
	new_bullet_timer:float  = 0
	allow_new_bullet:bool   = True
	background:object		= None
	background_rect:object  = None
	player_img:object       = None
	meteor_img:object       = None
	bullet_img:object       = None

mobs:list                 = []
bullets:list              = []
meteor_images:list        = []
new_bullet_interval:float = 0.2

def circle_collides(circle1:object, circle2:object) -> bool:
	# pygame.draw.circle(screen, (r,g,b), (x, y), R, w) #(r, g, b) is color, (x, y) is center, R is radius and w is the thickness of the circle border.
	# get distance between the circle's centers
	# use the Pythagorean Theorem to compute the distance
	distX = circle1.x - circle2.x
	distY = circle1.y - circle2.y
	distance = math.sqrt((distX * distX) + (distY * distY))	
	# if the distance is less than the sum of the circle's
	# radii, the circles are touching!
	r1 = circle1.radius
	r2 = circle2.radius
	if distance <= circle1.radius + circle2.radius:
		return True

	return False 

def collides(rect1:object, rect2:object) -> bool:
	''' check whether rectangles are NOT colliding '''
	if rect1.x > rect2.x + rect2.width or rect2.x > rect1.x + rect1.width:
		return False
	if rect1.y > rect2.y + rect2.height or rect2.y > rect1.y + rect1.height:
		return False
	return True

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

	return keystate, key_down, quit					# usage: if key_down == pygame.K_RETURN:, if keystate[pygame.K_UP]:

def check_mob_bullet_collisions(dt) -> None:
	''' check if any bullets are colliding with any Mobs '''
	for i in range(len(bullets) -1, -1, -1):
		destroy = False
		for j in range(len(mobs) -1, -1, -1):
			#destroy set to true if rectangles are colliding (bullet + Mob)
			try:
				#destroy = collides(bullets[i].get_rect() , mobs[j].get_rect())
				destroy = circle_collides(bullets[i].get_circle(), mobs[j].get_circle())
			except:
				pass				
			if destroy:
				bullets.pop()				
				mobs[j].reset()
				if len(bullets) == 0:
					break
	''' update bullets '''
	for i in range(len(bullets) - 1, -1, -1):
		if not bullets[i].update(dt):
			bullets.pop()	

def check_mob_player_collisions(keystate:object, dt:float) -> None:
	''' check if mob hit player '''
	player.update(keystate, dt)
	for mob in mobs:
		mob.update(dt)
		''' check if any mobs colliding with Player '''
		#if collides(mob.get_rect(), player.rect):
		if circle_collides(mob.get_circle(), player.circle):
			if not shared.debug:
				shared.gamestate = shared.gamestates['quit']

def shoot() -> None:
	''' Create a new bullet at player's position '''
	if data.allow_new_bullet:
		newBullet = bullet.Bullet(data.bullet_img, player.rect.centerx, player.rect.top)
		bullets.append(newBullet)
		data.new_bullet_timer = 0
		data.allow_new_bullet = False

def load_images() -> None:
	img_dir = os.path.join(shared.game_folder, "img")
	data.background = pygame.image.load(os.path.join(img_dir, "starfield.png")).convert()
	data.background_rect = data.background.get_rect()
	data.player_img = pygame.image.load(os.path.join(img_dir, "playerShip1_orange.png")).convert()
	data.meteor_img = pygame.image.load(os.path.join(img_dir, "meteorBrown_med1.png")).convert()
	data.bullet_img = pygame.image.load(os.path.join(img_dir, "laserRed16.png")).convert() 

	meteor_list = ['meteorBrown_big1.png', 'meteorBrown_big2.png', 'meteorBrown_med1.png', 'meteorBrown_med3.png',
				   'meteorBrown_small1.png', 'meteorBrown_small2.png', 'meteorBrown_tiny1.png']
	for img in meteor_list:
		meteor_images.append(pygame.image.load(os.path.join(img_dir, img)).convert())

def load() -> None:
	''' Setup pygame '''
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

	''' Load all game assets '''
	load_images()	

	player.init(data.player_img, 500, 0.5)
	for i in range(8):									# make 8 mobs
		mobs.append(mob.Mob(meteor_images))

	shared.debug = True

def update() -> None:
	'''
	Update all game items and keyboard input
	delta-time used in Love2D and Monogame is measured in seconds
	Can be obtained from clock.tick() in ms
	'''
	dt = shared.clock.tick(shared.FPS) / 1000				# update clock. dt can be passed to other update functions

	data.new_bullet_timer += dt
	if data.new_bullet_timer >= new_bullet_interval:
		data.allow_new_bullet = True
		data.new_bullet_timer = 0

	keystate, key_down, quit = process_events()				# get keypressed, keydown and close/esc user choice
	if quit:
		shared.gamestate = shared.gamestates["quit"]		# set gamestate to quit
	else:
		if shared.gamestate == shared.gamestates["play"]:
			if key_down == pygame.K_SPACE or keystate[pygame.K_SPACE]:
				shoot()
			check_mob_player_collisions(keystate, dt)
			check_mob_bullet_collisions(dt)					

def draw() -> None:
	''' Draw background and all active sprites '''
	shared.screen.fill(shared.BLACK)					# make screen black
	shared.screen.blit(data.background, data.background_rect)
	if shared.gamestate == shared.gamestates["play"]:
		player.draw()
		for mob in mobs:
			mob.draw()
		for bullet in bullets:
			bullet.draw()	

	pygame.display.flip()								# flip display to make it visible

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